---
title: include/iot-scurve-stepper.h


---

# include/iot-scurve-stepper.h








## Classes

|                | Name           |
| -------------- | -------------- |
| class | **[SCurveStepper](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md)**  |

## Types

|                | Name           |
| -------------- | -------------- |
| enum | **[SCurveStepperMotorState](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Files/iot-scurve-stepper_8h.md#enum-scurvesteppermotorstate)** { unknown, idle, speed_changing, cruise, failed } |










## Types Documentation

### enum SCurveStepperMotorState


| Enumerator | Value | Description |
| ---------- | ----- | ----------- |
| unknown |  |   |
| idle |  |   |
| speed_changing |  |   |
| cruise |  |   |
| failed |  |   |




































## Source code

```cpp
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
    double s0_pus;
    // motion: start setSpeed timestamp (chrono)
    std::chrono::microseconds motion_start;    

    // motion: speed variation (pulse/us)
    double s_d_pus;    
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
    double currentSpeed() const { return current_speed_pus * 1e-6 / pulse_rev; }
    //double currentPos() const { return current_pos_step; }
    int pulseExecuted() const { return pulse_executed; }
    int pulseExpected() const { return pulse_expected; }

    void setSpeed(double revSec, double durationSec);

    void setPos(double step, double durationSec);

    void control();

    void debugStats(bool block_on_error = true);
    
    double computeAccel(double s_d, double d, double t);
    double computeDuration(double s_d, double a);
    double computeSpeed(double s0, double s_d, double d, double t_r);
    double computePos(double s0, double p0, double s_d, double d, double t_r);
    double computePos(double s0, double p0, double s_d, double d);
    
};

#endif
```


-------------------------------

Updated on  4 January 2021 at 20:14:44 CET
