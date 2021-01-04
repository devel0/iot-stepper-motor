---
title: SCurveStepper
summary: s-curve shaped speed profile stepper motor control  

---

# SCurveStepper




s-curve shaped speed profile stepper motor control 

`#include <iot-scurve-stepper.h>`















## Public Functions

|                | Name           |
| -------------- | -------------- |
|  | **[SCurveStepper](classSCurveStepper.md#function-scurvestepper)**(int __tag, Timer & _timer, DigitalOut & _pulsePin, int _pulse_rev, std::chrono::microseconds _pulse_width_min =20us) <br>Construct a new [SCurveStepper](classSCurveStepper.md) object.  |
| int | **[tag](classSCurveStepper.md#function-tag)**() const <br>motor info tag  |
| int | **[phaseTag](classSCurveStepper.md#function-phasetag)**() const <br>current motion tag custom info  |
| void | **[setPhaseTag](classSCurveStepper.md#function-setphasetag)**(int _phase_tag) <br>set current motion tag custom info  |
| SCurveStepperMotorState | **[state](classSCurveStepper.md#function-state)**() const <br>current motor state  |
| std::chrono::microseconds | **[motionStart](classSCurveStepper.md#function-motionstart)**() const <br>latest motion start timestamp  |
| int | **[pulseRev](classSCurveStepper.md#function-pulserev)**() const <br>current pulse rev config  |
| double | **[currentSpeed](classSCurveStepper.md#function-currentspeed)**() const <br>actual motor speed  |
| int | **[pulseExecuted](classSCurveStepper.md#function-pulseexecuted)**() const <br>pulse executed last motion  |
| int | **[pulseExpected](classSCurveStepper.md#function-pulseexpected)**() const <br>pulse expected last motion  |
| void | **[setSpeed](classSCurveStepper.md#function-setspeed)**(double revSec, double durationSec) <br>change from current to given speed ( rev/sec ) in given duration time ( sec )  |
| void | **[setPos](classSCurveStepper.md#function-setpos)**(double step, double durationSec) <br>change from current pos to given one ( steps ) in given duration time ( sec )  |
| void | **[control](classSCurveStepper.md#function-control)**() <br>control process ( execute this in a priority loop )  |
| void | **[debugStats](classSCurveStepper.md#function-debugstats)**(bool block_on_error =true) <br>print to monitor some motion info  |
| double | **[computeAccel](classSCurveStepper.md#function-computeaccel)**(double s_d, double d, double t_r) <br>compute accel from given speed s_d, duration d and time t_r  |
| double | **[computeDuration](classSCurveStepper.md#function-computeduration)**(double s_d, double a) <br>compute duration from given speed s_d and acceleration a  |
| double | **[computeSpeed](classSCurveStepper.md#function-computespeed)**(double s0, double s_d, double d, double t_r) <br>compute speed from given initial speed s0, speed variation s_d (when t_r=d), duration d and time t_r  |
| double | **[computePos](classSCurveStepper.md#function-computepos)**(double s0, double p0, double s_d, double d, double t_r) <br>compute position from given initial speed s0, initial pos p0, speed variation s (when t_r=d), duration d and time t_r  |
| double | **[computePos](classSCurveStepper.md#function-computepos)**(double s0, double p0, double s_d, double d) <br>compute position from given initial speed s0, initial pos p0, speed variation s (when t_r=d), duration d  |





















## Public Functions Documentation

### function SCurveStepper

```cpp
SCurveStepper(
    int __tag,
    Timer & _timer,
    DigitalOut & _pulsePin,
    int _pulse_rev,
    std::chrono::microseconds _pulse_width_min =20us
)
```

Construct a new [SCurveStepper](classSCurveStepper.md) object. 

**Parameters**: 

  * **__tag** motor custom tag 
  * **_timer** shared timer 
  * **_pulsePin** pulse pin (DigitalOut) 
  * **_pulse_rev** pulse/rev (external driver setting) 
  * **_pulse_width_min** min pulse width (external driver setting) 




























### function tag

```cpp
inline int tag() const
```

motor info tag 







**Return**: int 





















### function phaseTag

```cpp
inline int phaseTag() const
```

current motion tag custom info 







**Return**: int 





















### function setPhaseTag

```cpp
inline void setPhaseTag(
    int _phase_tag
)
```

set current motion tag custom info 

**Parameters**: 

  * **_phase_tag** phase tag to set 




























### function state

```cpp
inline SCurveStepperMotorState state() const
```

current motor state 







**Return**: SCurveStepperMotorState 





















### function motionStart

```cpp
inline std::chrono::microseconds motionStart() const
```

latest motion start timestamp 







**Return**: std::chrono::microseconds 





















### function pulseRev

```cpp
inline int pulseRev() const
```

current pulse rev config 







**Return**: int 





















### function currentSpeed

```cpp
inline double currentSpeed() const
```

actual motor speed 







**Return**: double (rev/sec) 





















### function pulseExecuted

```cpp
inline int pulseExecuted() const
```

pulse executed last motion 







**Return**: int 





















### function pulseExpected

```cpp
inline int pulseExpected() const
```

pulse expected last motion 







**Return**: int 





















### function setSpeed

```cpp
void setSpeed(
    double revSec,
    double durationSec
)
```

change from current to given speed ( rev/sec ) in given duration time ( sec ) 

**Parameters**: 

  * **revSec** target speed (rev/sec) 
  * **durationSec** duration to reach target speed (s) 




























### function setPos

```cpp
void setPos(
    double step,
    double durationSec
)
```

change from current pos to given one ( steps ) in given duration time ( sec ) 

**Parameters**: 

  * **step** position to go to (steps) 
  * **durationSec** duration to reach target position (s) 




























### function control

```cpp
void control()
```

control process ( execute this in a priority loop ) 




























### function debugStats

```cpp
void debugStats(
    bool block_on_error =true
)
```

print to monitor some motion info 

**Parameters**: 

  * **block_on_error** if true and motion state failed program blocks 




























### function computeAccel

```cpp
double computeAccel(
    double s_d,
    double d,
    double t_r
)
```

compute accel from given speed s_d, duration d and time t_r 

**Parameters**: 

  * **s_d** targe speed when t=d 
  * **d** duration 
  * **t_r** time where compute accel 







**Return**: double 





















### function computeDuration

```cpp
double computeDuration(
    double s_d,
    double a
)
```

compute duration from given speed s_d and acceleration a 

**Parameters**: 

  * **s_d** target speed when t=d 
  * **a** acceleration 







**Return**: double 





















### function computeSpeed

```cpp
double computeSpeed(
    double s0,
    double s_d,
    double d,
    double t_r
)
```

compute speed from given initial speed s0, speed variation s_d (when t_r=d), duration d and time t_r 

**Parameters**: 

  * **s0** initial speed 
  * **s_d** target speed when t=d 
  * **d** durtion 
  * **t_r** time where compute speed 







**Return**: double 





















### function computePos

```cpp
double computePos(
    double s0,
    double p0,
    double s_d,
    double d,
    double t_r
)
```

compute position from given initial speed s0, initial pos p0, speed variation s (when t_r=d), duration d and time t_r 

**Parameters**: 

  * **s0** initial speed 
  * **p0** initial position 
  * **s_d** target speed when t=d 
  * **d** duration 
  * **t_r** time where compute speed 







**Return**: double 





















### function computePos

```cpp
double computePos(
    double s0,
    double p0,
    double s_d,
    double d
)
```

compute position from given initial speed s0, initial pos p0, speed variation s (when t_r=d), duration d 

**Parameters**: 

  * **s0** initial speed 
  * **p0** initial position 
  * **s_d** target speed when t=d 
  * **d** duration 







**Return**: double 





























-------------------------------

Updated on  4 January 2021 at 08:20:30 CET