#ifndef _IOT_SCURVE_STEPPER_H
#define _IOT_SCURVE_STEPPER_H

#include <mbed.h>

enum SCurveStepperMotorState
{
    unknown,
    idle,
    speed_changing,
    cruise,
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
    Timeout pulse_down;        

    // current position (step)
    double current_pos_step;
    // previous position (step)
    double debug_prev_pos_step;
    // actual speed (pulse/us)
    double current_speed_pus;

    // actual pulse period (us)
    double current_period_us;
    // actual pulse period start timestamp (chrono)
    std::chrono::microseconds current_period_start;

    // motion: start timestamp (us)
    double t0_us;
    // motion: start pos
    double p0_step;    
    // motion: start speed (pulse/us)
    double s0_speed_pus;
    // motion: start setSpeed timestamp (chrono)
    std::chrono::microseconds motion_start;    

    // motion: speed variation (pulse/us)
    double s_speed_pus;    
    // motion: duration (us)
    double d_us;
    // motion: step expected till now
    int pulse_expected;
    // motion: total step expected at end of motion variation
    int pulse_expected_max;
    // motion: step executed till now
    int pulse_executed;    
    // motion: step excees till lnow
    int pulse_excees;

    // motion count
    int motion_count;    
    // min period registered during motion controls
    double period_min_us;    

    // 2 * pi
    static const double _2PI;
    // pi^2
    static const double _PI2;

    void pulseDownFn();

public:
    SCurveStepper(int __tag, Timer &_timer, DigitalOut &_pulsePin, int _pulse_rev,
                  std::chrono::microseconds _pulse_width_min = 20us);

    int tag() const { return _tag; }
    SCurveStepperMotorState state() const { return _state; }
    std::chrono::microseconds motionStart() const { return motion_start; }    
    int pulseRev() const { return pulse_rev; }
    /// current speed ( rev/sec )
    double currentSpeed() const { return current_speed_pus * 1e-6 / pulse_rev; }
    /// pulse executed last motion
    int pulseExecuted() const { return pulse_executed; }
    /// pulse expected last motion
    int pulseExpected() const { return pulse_expected; }

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
