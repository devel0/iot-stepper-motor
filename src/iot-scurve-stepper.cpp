#include <iot-scurve-stepper.h>

#include <constant-utils.h>
#include <timer-utils.h>

const double SCurveStepper::_2PI = 2 * PI;
const double SCurveStepper::_PI2 = PI * PI;

SCurveStepper::SCurveStepper(int __tag,
                             Timer &_timer,
                             DigitalOut &_pulse_pin,
                             int _pulse_rev,
                             std::chrono::microseconds _pulse_width_min) : timer(_timer), pulse_pin(_pulse_pin)

{
    _tag = __tag;
    pulse_rev = _pulse_rev;
    pulse_width_min = _pulse_width_min;

    current_speed_pus = 0;
    current_pos_step = 0;
    p0_step = 0;
    pulse_pin = 0;
    pulse_excees = 0;
    period_min_us = 0;
}
int k = 0;
int f = 0;

void SCurveStepper::setSpeed(double speed_rev_sec, double durationSec)
{
    auto cur_t = timer.elapsed_time();

    s0_speed_pus = current_speed_pus;
    p0_step = current_pos_step;
    d_us = durationSec * 1e6;
    t0_us = chrono_us(cur_t);
    pulse_executed = 0;
    pulse_expected = 0;
    pulse_excees = 0;
    s_speed_pus = speed_rev_sec * pulse_rev * 1e-6 - s0_speed_pus;
    pulse_expected_max = round(computePos(s0_speed_pus, p0_step, s_speed_pus, d_us)) - p0_step;
    if (_state == SCurveStepperMotorState::unknown)
        period_min_us = d_us;
    _state = SCurveStepperMotorState::speed_changing;
    motion_start = cur_t;
    current_period_start = cur_t;

    k = 0;
    if (f==1)
    {
        f=f;
    }
    ++f;
}

void SCurveStepper::pulseDownFn()
{
    pulse_pin = 0;
}

void SCurveStepper::control()
{
    if (_state == SCurveStepperMotorState::idle || _state == SCurveStepperMotorState::unknown)
        return;

    auto cur_t = timer.elapsed_time();

    double cur_t_us = chrono_us(cur_t);
    double t_us = cur_t_us - chrono_us(motion_start);

    if (t_us >= d_us && pulse_executed >= pulse_expected_max)
        _state = SCurveStepperMotorState::cruise;

    if (f==3)
    {
        f=f;
    }

    switch (_state)
    {
    case SCurveStepperMotorState::cruise:
    {
        if (current_speed_pus != 0)
        {
            current_period_us = 1.0 / current_speed_pus;

            if (pulse_pin != 1)
            {
                auto now_period_us = chrono_us(cur_t - current_period_start);

                double rp = current_period_us;

                if (now_period_us >= rp)
                {
                    if (now_period_us < period_min_us)
                    {
                        period_min_us = now_period_us;
                    }

                    current_period_start = cur_t;
                    pulse_pin = 1;
                    pulse_down.attach(callback(this, &SCurveStepper::pulseDownFn), pulse_width_min);
                    ++current_pos_step;
                }
            }
        }
    }
    break;

    case SCurveStepperMotorState::speed_changing:
    {
        if (f==2)
        {
            f=f;
        }
        current_speed_pus = computeSpeed(s0_speed_pus, s_speed_pus, d_us, t_us);

        pulse_expected = round(computePos(s0_speed_pus, p0_step, s_speed_pus, d_us, t_us)) - p0_step;

        if (t_us > d_us / 2 && f==2)
        {
            k = k;
        }

        if (current_speed_pus != 0)
        {
            current_period_us = 1.0 / current_speed_pus;

            if (pulse_pin != 1)
            {
                k++;

                bool time_exceeded = t_us > d_us;
                auto now_period_us = chrono_us(cur_t - current_period_start);
                // adjust
                double rp = current_period_us;
                if (pulse_executed < pulse_expected)
                    rp /= 2;
                if (t_us >= d_us)
                {
                    k = k;
                }
                if (now_period_us >= rp)
                {
                    if (s_speed_pus == 0 && t_us > d_us / 2)
                    {
                        k = k;
                    }

                    if (time_exceeded)
                        ++pulse_excees;

                    if (pulse_excees > 1)
                    {
                        motion_end = cur_t;
                        _state = SCurveStepperMotorState::failed;
                        ++motion_count;
                        return;
                    }

                    if (pulse_executed < pulse_expected_max)
                    {
                        if (now_period_us < period_min_us)
                        {
                            period_min_us = now_period_us;
                        }

                        current_period_start = cur_t;
                        pulse_pin = 1;
                        pulse_down.attach(callback(this, &SCurveStepper::pulseDownFn), pulse_width_min);
                        if (s0_speed_pus > s_speed_pus)
                            --current_pos_step;
                        else
                            ++current_pos_step;
                        ++pulse_executed;
                    }
                }
            }
        }
    }
    break;
    }
}

void SCurveStepper::debugStats(bool block_on_error)
{
    printf("debug stats\n");
    // auto effective_motion_duration = motion_end - motion_start;

    // uint64_t mdur_ms = chrono_ms(effective_motion_duration);

    // printf("m[%d] pulse_executed: %d   pulse_expected: %d   motion_dur: %llums   period_min: %lluus\n",
    //        _tag,
    //        pulse_executed,
    //        pulse_expected,
    //        mdur_ms,
    //        period_min_us);

    // if (block_on_error && pulse_executed != pulse_expected)
    // {
    //     printf("ERROR\n");
    //     while (1)
    //         ;
    // }
}

double SCurveStepper::computeAccel(double s, double d, double t)
{
    return s / d * (1 - cos(t / d * _2PI));
}

double SCurveStepper::computeDuration(double s, double a)
{
    return 2 * s / a;
}

double SCurveStepper::computeSpeed(double s0, double s, double d, double t)
{
    return -0.5 * sin(_2PI * t / d) * d * s / (d * PI) + s * t / d + s0;
}

double SCurveStepper::computePos(double s0, double p0, double s, double d, double t)
{
    return (.25 * cos(_2PI * t / d) * d * d / (d * _PI2) - .25 * d / _PI2 + .5 * t * t / d) * s + p0 + s0 * t;
}

double SCurveStepper::computePos(double s0, double p0, double s, double d)
{
    return d * (0.5 * s + s0) + p0;
}