---
title: SCurveStepper


---

# SCurveStepper





















## Public Functions

|                | Name           |
| -------------- | -------------- |
|  | **[SCurveStepper](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-scurvestepper)**(int __tag, Timer & _timer, DigitalOut & _pulsePin, int _pulse_rev, std::chrono::microseconds _pulse_width_min =20us)  |
| int | **[tag](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-tag)**() const  |
| SCurveStepperMotorState | **[state](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-state)**() const  |
| std::chrono::microseconds | **[motionStart](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-motionstart)**() const  |
| int | **[pulseRev](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-pulserev)**() const  |
| double | **[currentSpeed](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-currentspeed)**() const <br>current speed ( rev/sec )  |
| int | **[pulseExecuted](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-pulseexecuted)**() const <br>current pos ( step )  |
| int | **[pulseExpected](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-pulseexpected)**() const <br>pulse expected last motion  |
| void | **[setSpeed](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-setspeed)**(double revSec, double durationSec) <br>change from current to given speed ( rev/sec ) in given duration time ( sec )  |
| void | **[setPos](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-setpos)**(double step, double durationSec) <br>change from current pos to given one ( steps ) in given duration time ( sec )  |
| void | **[control](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-control)**()  |
| void | **[debugStats](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-debugstats)**(bool block_on_error =true)  |
| double | **[computeAccel](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-computeaccel)**(double s_d, double d, double t) <br>compute accel from given speed s_d, duration d and time t_r  |
| double | **[computeDuration](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-computeduration)**(double s_d, double a) <br>compute duration from given speed s_d and acceleration a  |
| double | **[computeSpeed](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-computespeed)**(double s0, double s_d, double d, double t_r) <br>compute speed from given initial speed s0, speed variation s_d (when t_r=d), duration d and time t_r  |
| double | **[computePos](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-computepos)**(double s0, double p0, double s_d, double d, double t_r) <br>compute position from given initial speed s0, initial pos p0, speed variation s (when t_r=d), duration d and time t_r  |
| double | **[computePos](https://github.com/devel0/iot-stepper-motor/tree/main/data/api/Classes/classSCurveStepper.md#function-computepos)**(double s0, double p0, double s_d, double d) <br>compute position from given initial speed s0, initial pos p0, speed variation s (when t_r=d), duration d  |





















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





























### function tag

```cpp
inline int tag() const
```





























### function state

```cpp
inline SCurveStepperMotorState state() const
```





























### function motionStart

```cpp
inline std::chrono::microseconds motionStart() const
```





























### function pulseRev

```cpp
inline int pulseRev() const
```





























### function currentSpeed

```cpp
inline double currentSpeed() const
```

current speed ( rev/sec ) 




























### function pulseExecuted

```cpp
inline int pulseExecuted() const
```

current pos ( step ) 


























pulse executed last motion 


### function pulseExpected

```cpp
inline int pulseExpected() const
```

pulse expected last motion 




























### function setSpeed

```cpp
void setSpeed(
    double revSec,
    double durationSec
)
```

change from current to given speed ( rev/sec ) in given duration time ( sec ) 




























### function setPos

```cpp
void setPos(
    double step,
    double durationSec
)
```

change from current pos to given one ( steps ) in given duration time ( sec ) 




























### function control

```cpp
void control()
```





























### function debugStats

```cpp
void debugStats(
    bool block_on_error =true
)
```





























### function computeAccel

```cpp
double computeAccel(
    double s_d,
    double d,
    double t
)
```

compute accel from given speed s_d, duration d and time t_r 




























### function computeDuration

```cpp
double computeDuration(
    double s_d,
    double a
)
```

compute duration from given speed s_d and acceleration a 




























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




































-------------------------------

Updated on  4 January 2021 at 20:14:44 CET