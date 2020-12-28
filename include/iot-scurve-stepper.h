#ifndef _IOT_SCURVE_STEPPER_H
#define _IOT_SCURVE_STEPPER_H

#include <mbed.h>

enum SCurveStepperMotorState
{
    unknown,
    idle,
    moving,
    failed
};

class SCurveStepper
{
    int _tag;
    Timer &timer;
    DigitalOut &pulse_pin;
    int pulse_rev;
    std::chrono::microseconds pulse_width_min;

    SCurveStepperMotorState _state;
    double current_speed_ps;
    double current_pos_step;
    double s0_speed_ps;
    double d_us;
    double t0_us;
    int pulse_expected;
    int pulse_executed;
    int pulse_total;

    /// 2 * pi
    double _2PI;

    /// pi^2
    double _PI2;

public:
    SCurveStepper(int __tag, Timer &_timer, DigitalOut &_pulsePin, int _pulse_rev,
                  std::chrono::microseconds _pulse_width_min = 20us);

    int tag() const { return _tag; }
    SCurveStepperMotorState state() const { return _state; }
    int pulseRev() const { return pulse_rev; }
    /// current speed ( rev/sec )
    double currentSpeed() const { return current_speed_ps / pulse_rev; }

    /// change from current to given speed ( rev/sec ) in given duration time ( sec )
    void setSpeed(double revSec, double durationSec);

    void control();

    void debugStats(bool block_on_error = true);
    /// compute accel from given speed s, duration d and time t
    double computeAccel(double s, double d, double t);
    /// compute duration from given speed s and acceleration a
    double computeDuration(double s, double a);
    /// compute speed from given initial speed s0, speed variation s (when t=d), duration d and time t
    double computeSpeed(double s0, double s, double d, double t);
    /// compute position from given initial speed s0, initial pos p0, speed variation s (when t=d), duration d and time t
    double computePos(double s0, double p0, double s, double d, double t);
    /// compute position from given initial speed s0, initial pos p0, speed variation s (when t=d), duration d
    double computePos(double s0, double p0, double s, double d);
    
};

#endif
