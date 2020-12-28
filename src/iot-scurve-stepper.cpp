#include <iot-scurve-stepper.h>

#include <constant-utils.h>

SCurveStepper::SCurveStepper(int __tag,
                             Timer &_timer,
                             DigitalOut &_pulse_pin,
                             int _pulse_rev,
                             std::chrono::microseconds _pulse_width_min) : timer(_timer), pulse_pin(_pulse_pin)

{
    _tag = __tag;
    pulse_rev = _pulse_rev;
    pulse_width_min = _pulse_width_min;

    current_speed_ps = 0;
    current_pos_step = 0;

    _2PI = 2 * PI;
    _PI2 = PI * PI;
}

void SCurveStepper::setSpeed(double revSec, double durationSec)
{
    s0_speed_ps = current_speed_ps;
    d_us = durationSec * 1e6;
    t0_us = timer.elapsed_time().count();
    pulse_executed = 0;
    pulse_expected = 0;
    double revCount = computePos(s0_speed_ps, current_pos_step, revSec * pulse_rev - s0_speed_ps, d_us);
    pulse_total = round(revCount * pulse_rev);

    _state = SCurveStepperMotorState::moving;
}

void SCurveStepper::control()
{
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