# Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`class `[`SCurveStepper`](#classSCurveStepper) | s-curve shaped speed profile stepper motor control

# class `SCurveStepper` 

s-curve shaped speed profile stepper motor control

## Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public  `[`SCurveStepper`](#classSCurveStepper_1ac248be79ec419ec7ffde3baf02aade3a)`(int __tag,Timer & _timer,DigitalOut & _pulsePin,int _pulse_rev,std::chrono::microseconds _pulse_width_min)` | Construct a new [SCurveStepper](#classSCurveStepper) object.
`public inline int `[`tag`](#classSCurveStepper_1a061178126e2cc6ac32b663817dc3978d)`() const` | motor info tag
`public inline int `[`phaseTag`](#classSCurveStepper_1adaf678d9a02d6bd854d887ca0c41220b)`() const` | current motion tag custom info
`public inline void `[`setPhaseTag`](#classSCurveStepper_1afed4cbfe17fa2524cb0d94936c7ae91f)`(int _phase_tag)` | set current motion tag custom info
`public inline SCurveStepperMotorState `[`state`](#classSCurveStepper_1afdca39e47cd81312a8a3fe01e48543d0)`() const` | current motor state
`public inline std::chrono::microseconds `[`motionStart`](#classSCurveStepper_1ad436ceefb5219441ffb3c58b2ba18803)`() const` | latest motion start timestamp
`public inline int `[`pulseRev`](#classSCurveStepper_1af331ebfd1739f43231d317ab270ae4d4)`() const` | current pulse rev config
`public inline double `[`currentSpeed`](#classSCurveStepper_1a85790885dc8ff01f8b3e459c336f3b39)`() const` | actual motor speed
`public inline int `[`pulseExecuted`](#classSCurveStepper_1af2618620497192b2c70ca6a5725cc4bb)`() const` | pulse executed last motion
`public inline int `[`pulseExpected`](#classSCurveStepper_1a1eff6fe57ae3735b43f0d62ad3f859c5)`() const` | pulse expected last motion
`public void `[`setSpeed`](#classSCurveStepper_1ac5cbab10481b4f39c8ee7529c6c8d922)`(double revSec,double durationSec)` | change from current to given speed ( rev/sec ) in given duration time ( sec )
`public void `[`setPos`](#classSCurveStepper_1a5afb6742d5de563ca1deca5608402545)`(double step,double durationSec)` | change from current pos to given one ( steps ) in given duration time ( sec )
`public void `[`control`](#classSCurveStepper_1a1fd0e0383171f9446d4769b4a9155b8c)`()` | control process ( execute this in a priority loop )
`public void `[`debugStats`](#classSCurveStepper_1a1701f0b71ddb725109cf40d12927c38b)`(bool block_on_error)` | print to monitor some motion info
`public double `[`computeAccel`](#classSCurveStepper_1a93061c82426075b830988157df085bb5)`(double s_d,double d,double t_r)` | compute accel from given speed s_d, duration d and time t_r
`public double `[`computeDuration`](#classSCurveStepper_1a7af3d4c0287ce78510a5a66df8a764a7)`(double s_d,double a)` | compute duration from given speed s_d and acceleration a
`public double `[`computeSpeed`](#classSCurveStepper_1a98235a25b57823667cabada84cc34165)`(double s0,double s_d,double d,double t_r)` | compute speed from given initial speed s0, speed variation s_d (when t_r=d), duration d and time t_r
`public double `[`computePos`](#classSCurveStepper_1a0b57185511a68fbd370d04e1bce583c6)`(double s0,double p0,double s_d,double d,double t_r)` | compute position from given initial speed s0, initial pos p0, speed variation s (when t_r=d), duration d and time t_r
`public double `[`computePos`](#classSCurveStepper_1a69d34a3841019246b76b8a701002665b)`(double s0,double p0,double s_d,double d)` | compute position from given initial speed s0, initial pos p0, speed variation s (when t_r=d), duration d

## Members

#### `public  `[`SCurveStepper`](#classSCurveStepper_1ac248be79ec419ec7ffde3baf02aade3a)`(int __tag,Timer & _timer,DigitalOut & _pulsePin,int _pulse_rev,std::chrono::microseconds _pulse_width_min)` 

Construct a new [SCurveStepper](#classSCurveStepper) object.

#### Parameters
* `__tag` motor custom tag 

* `_timer` shared timer 

* `_pulsePin` pulse pin (DigitalOut) 

* `_pulse_rev` pulse/rev (external driver setting) 

* `_pulse_width_min` min pulse width (external driver setting)

#### `public inline int `[`tag`](#classSCurveStepper_1a061178126e2cc6ac32b663817dc3978d)`() const` 

motor info tag

#### Returns
int

#### `public inline int `[`phaseTag`](#classSCurveStepper_1adaf678d9a02d6bd854d887ca0c41220b)`() const` 

current motion tag custom info

#### Returns
int

#### `public inline void `[`setPhaseTag`](#classSCurveStepper_1afed4cbfe17fa2524cb0d94936c7ae91f)`(int _phase_tag)` 

set current motion tag custom info

#### Parameters
* `_phase_tag` phase tag to set

#### `public inline SCurveStepperMotorState `[`state`](#classSCurveStepper_1afdca39e47cd81312a8a3fe01e48543d0)`() const` 

current motor state

#### Returns
SCurveStepperMotorState

#### `public inline std::chrono::microseconds `[`motionStart`](#classSCurveStepper_1ad436ceefb5219441ffb3c58b2ba18803)`() const` 

latest motion start timestamp

#### Returns
std::chrono::microseconds

#### `public inline int `[`pulseRev`](#classSCurveStepper_1af331ebfd1739f43231d317ab270ae4d4)`() const` 

current pulse rev config

#### Returns
int

#### `public inline double `[`currentSpeed`](#classSCurveStepper_1a85790885dc8ff01f8b3e459c336f3b39)`() const` 

actual motor speed

#### Returns
double (rev/sec)

#### `public inline int `[`pulseExecuted`](#classSCurveStepper_1af2618620497192b2c70ca6a5725cc4bb)`() const` 

pulse executed last motion

#### Returns
int

#### `public inline int `[`pulseExpected`](#classSCurveStepper_1a1eff6fe57ae3735b43f0d62ad3f859c5)`() const` 

pulse expected last motion

#### Returns
int

#### `public void `[`setSpeed`](#classSCurveStepper_1ac5cbab10481b4f39c8ee7529c6c8d922)`(double revSec,double durationSec)` 

change from current to given speed ( rev/sec ) in given duration time ( sec )

#### Parameters
* `revSec` target speed (rev/sec) 

* `durationSec` duration to reach target speed (s)

#### `public void `[`setPos`](#classSCurveStepper_1a5afb6742d5de563ca1deca5608402545)`(double step,double durationSec)` 

change from current pos to given one ( steps ) in given duration time ( sec )

#### Parameters
* `step` position to go to (steps) 

* `durationSec` duration to reach target position (s)

#### `public void `[`control`](#classSCurveStepper_1a1fd0e0383171f9446d4769b4a9155b8c)`()` 

control process ( execute this in a priority loop )

#### `public void `[`debugStats`](#classSCurveStepper_1a1701f0b71ddb725109cf40d12927c38b)`(bool block_on_error)` 

print to monitor some motion info

#### Parameters
* `block_on_error` if true and motion state failed program blocks

#### `public double `[`computeAccel`](#classSCurveStepper_1a93061c82426075b830988157df085bb5)`(double s_d,double d,double t_r)` 

compute accel from given speed s_d, duration d and time t_r

#### Parameters
* `s_d` targe speed when t=d 

* `d` duration 

* `t_r` time where compute accel 

#### Returns
double

#### `public double `[`computeDuration`](#classSCurveStepper_1a7af3d4c0287ce78510a5a66df8a764a7)`(double s_d,double a)` 

compute duration from given speed s_d and acceleration a

#### Parameters
* `s_d` target speed when t=d 

* `a` acceleration 

#### Returns
double

#### `public double `[`computeSpeed`](#classSCurveStepper_1a98235a25b57823667cabada84cc34165)`(double s0,double s_d,double d,double t_r)` 

compute speed from given initial speed s0, speed variation s_d (when t_r=d), duration d and time t_r

#### Parameters
* `s0` initial speed 

* `s_d` target speed when t=d 

* `d` durtion 

* `t_r` time where compute speed 

#### Returns
double

#### `public double `[`computePos`](#classSCurveStepper_1a0b57185511a68fbd370d04e1bce583c6)`(double s0,double p0,double s_d,double d,double t_r)` 

compute position from given initial speed s0, initial pos p0, speed variation s (when t_r=d), duration d and time t_r

#### Parameters
* `s0` initial speed 

* `p0` initial position 

* `s_d` target speed when t=d 

* `d` duration 

* `t_r` time where compute speed 

#### Returns
double

#### `public double `[`computePos`](#classSCurveStepper_1a69d34a3841019246b76b8a701002665b)`(double s0,double p0,double s_d,double d)` 

compute position from given initial speed s0, initial pos p0, speed variation s (when t_r=d), duration d

#### Parameters
* `s0` initial speed 

* `p0` initial position 

* `s_d` target speed when t=d 

* `d` duration 

#### Returns
double

Generated by [Moxygen](https://sourcey.com/moxygen)