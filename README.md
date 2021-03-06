# iot-stepper-motor

[API](data/api/Classes/classSCurveStepper.md)

<hr/>

<!-- TOC -->
* [Description](#description)
* [Quickstart](#quickstart)
* [Analysis](#analysis)
  + [Motion equations](#motion-equations)
  + [Plot analysis (oxyplot)](#plot-analysis-oxyplot)
* [Example01](#example01)
  + [logic analyzer](#logic-analyzer)
  + [video](#video)
  + [code](#code)
  + [plot (xlsx)](#plot-xlsx)
  + [monitor](#monitor)
* [Examples wirings](#examples-wirings)
    - [nucleo-64 **stm32-f446re**](#nucleo-64-stm32-f446re)
    - [nucleo-144 **stm32-f767zi**](#nucleo-144-stm32-f767zi)
* [TODO](#todo)
* [Debugging](#debugging)
* [How this project was built](#how-this-project-was-built)
<!-- TOCEND -->

## Description

:warning: this is a work in progress

Library to control stepper motor scurve motion type for ststm32 platform, mbed os framework.

## Quickstart

Install using vscode command palette `PlatformIO: New Terminal` referring to [platformio registry](https://platformio.org/lib/show/11637/iot-stepper-motor) cli mode doc.

If got trouble during compile, remove `.pio/libdeps/nucleo_f446re/iot-stepper-motor/library.json`.

## Analysis

```sh
git submodule update --init --recursive
cd analysis
dotnet run
```

### Motion equations

For symbolic calculus [AngouriMath](https://github.com/asc-community/AngouriMath) library was used.

- vars ( code refs [a][1] ; [aMax][2] ; [s][3] ; [s<sub>d</sub>][4] ; [x][5] ; [x<sub>d</sub>][6] )
    - **t** (time) ; **t<sub>0</sub>** (initial time) ; **t<sub>r</sub>** (time relative to t<sub>0</sub>)    
    - **a** (acceleration) ; **a<sub>max</sub>** (max accel when t=d/2)
    - **s** (speed) ; **s<sub>0</sub>** (initial speed) ; **s<sub>d</sub>** (speed at end of motion when t=d)
    - **x** (position) ; **x<sub>0</sub>** (initial position) ; **s<sub>d</sub>** (position at end of motion when t=d)

[1]: https://github.com/devel0/iot-stepper-motor/blob/0a21b306125915c26b5c39f992fe5b0b4b8aa9dc/analysis/Program.cs#L152-L154
[2]: https://github.com/devel0/iot-stepper-motor/blob/0a21b306125915c26b5c39f992fe5b0b4b8aa9dc/analysis/Program.cs#L170
[3]: https://github.com/devel0/iot-stepper-motor/blob/0a21b306125915c26b5c39f992fe5b0b4b8aa9dc/analysis/Program.cs#L178
[4]: https://github.com/devel0/iot-stepper-motor/blob/0a21b306125915c26b5c39f992fe5b0b4b8aa9dc/analysis/Program.cs#L194
[5]: https://github.com/devel0/iot-stepper-motor/blob/0a21b306125915c26b5c39f992fe5b0b4b8aa9dc/analysis/Program.cs#L186
[6]: https://github.com/devel0/iot-stepper-motor/blob/0a21b306125915c26b5c39f992fe5b0b4b8aa9dc/analysis/Program.cs#L202

<!-- $$
\large
t_r = t - t_0
\quad
s_d = s(d)
$$ --> 

<div align="center"><img style="background: white;" src="https://render.githubusercontent.com/render/math?math=%5Clarge%0At_r%20%3D%20t%20-%20t_0%0A%5Cquad%0As_d%20%3D%20s(d)"></div>

<!-- $$
\large
a(t_r)=\frac{s_{d}}{d}\cdot \left(1-\cos\left(\frac{t_r}{d}\cdot 2\cdot \pi\right)\right)
$$ --> 

<br/>
<div align="center"><img style="background: white;" src="https://render.githubusercontent.com/render/math?math=%5Clarge%0Aa(t_r)%3D%5Cfrac%7Bs_%7Bd%7D%7D%7Bd%7D%5Ccdot%20%5Cleft(1-%5Ccos%5Cleft(%5Cfrac%7Bt_r%7D%7Bd%7D%5Ccdot%202%5Ccdot%20%5Cpi%5Cright)%5Cright)"></div>

<!-- $$
\large
a_{max}=a\left(\frac{d}{2}\right)
\quad
d = \left\{ \frac{2\cdot s_{d}}{a_{max}} \right\}
$$ --> 

<br/>
<div align="center"><img style="background: white;" src="https://render.githubusercontent.com/render/math?math=%5Clarge%0Aa_%7Bmax%7D%3Da%5Cleft(%5Cfrac%7Bd%7D%7B2%7D%5Cright)%0A%5Cquad%0Ad%20%3D%20%5Cleft%5C%7B%20%5Cfrac%7B2%5Ccdot%20s_%7Bd%7D%7D%7Ba_%7Bmax%7D%7D%20%5Cright%5C%7D"></div>

<!-- $$
\large
s(t_r)=\frac{\frac{-1}{2}\cdot \sin\left(\frac{2\cdot \pi\cdot t_r}{d}\right)\cdot d\cdot s_{d}}{d\cdot \pi}+\frac{s_{d}\cdot t_r}{d}+s_{0}
$$ --> 

<br/>
<div align="center"><img style="background: white;" src="https://render.githubusercontent.com/render/math?math=%5Clarge%0As(t_r)%3D%5Cfrac%7B%5Cfrac%7B-1%7D%7B2%7D%5Ccdot%20%5Csin%5Cleft(%5Cfrac%7B2%5Ccdot%20%5Cpi%5Ccdot%20t_r%7D%7Bd%7D%5Cright)%5Ccdot%20d%5Ccdot%20s_%7Bd%7D%7D%7Bd%5Ccdot%20%5Cpi%7D%2B%5Cfrac%7Bs_%7Bd%7D%5Ccdot%20t_r%7D%7Bd%7D%2Bs_%7B0%7D"></div>

<!-- $$
\large
x(t_r)=x_{0}+\frac{\frac{1}{4}\cdot \cos\left(\frac{2\cdot \pi\cdot t_r}{d}\right)\cdot {d}^{2}\cdot s_{d}}{d\cdot {\pi}^{2}}+\frac{\frac{1}{2}\cdot s_{d}\cdot {t_r}^{2}}{d}+s_{0}\cdot t_r-\frac{\frac{1}{4}\cdot d\cdot s_{d}}{{\pi}^{2}}
$$ --> 

<br/>
<div align="center"><img style="background: white;" src="https://render.githubusercontent.com/render/math?math=%5Clarge%0Ax(t_r)%3Dx_%7B0%7D%2B%5Cfrac%7B%5Cfrac%7B1%7D%7B4%7D%5Ccdot%20%5Ccos%5Cleft(%5Cfrac%7B2%5Ccdot%20%5Cpi%5Ccdot%20t_r%7D%7Bd%7D%5Cright)%5Ccdot%20%7Bd%7D%5E%7B2%7D%5Ccdot%20s_%7Bd%7D%7D%7Bd%5Ccdot%20%7B%5Cpi%7D%5E%7B2%7D%7D%2B%5Cfrac%7B%5Cfrac%7B1%7D%7B2%7D%5Ccdot%20s_%7Bd%7D%5Ccdot%20%7Bt_r%7D%5E%7B2%7D%7D%7Bd%7D%2Bs_%7B0%7D%5Ccdot%20t_r-%5Cfrac%7B%5Cfrac%7B1%7D%7B4%7D%5Ccdot%20d%5Ccdot%20s_%7Bd%7D%7D%7B%7B%5Cpi%7D%5E%7B2%7D%7D"></div>

<!-- $$
\large
s(d)=\left\{ \frac{2\cdot \left(-d\cdot s_{0}-x_{0}+x_{d}\right)}{d} \right\}
$$ --> 

<br/>
<div align="center"><img style="background: white;" src="https://render.githubusercontent.com/render/math?math=%5Clarge%0As(d)%3D%5Cleft%5C%7B%20%5Cfrac%7B2%5Ccdot%20%5Cleft(-d%5Ccdot%20s_%7B0%7D-x_%7B0%7D%2Bx_%7Bd%7D%5Cright)%7D%7Bd%7D%20%5Cright%5C%7D"></div>

<!-- $$
\large
x(d)=d\cdot \left(s_{0}+\frac{1}{2}\cdot s_{d}\right)+x_{0}
$$ --> 

<br/>
<div align="center"><img style="background: white;" src="https://render.githubusercontent.com/render/math?math=%5Clarge%0Ax(d)%3Dd%5Ccdot%20%5Cleft(s_%7B0%7D%2B%5Cfrac%7B1%7D%7B2%7D%5Ccdot%20s_%7Bd%7D%5Cright)%2Bx_%7B0%7D"></div>

### Plot analysis (oxyplot)

Follow graph shows the example described in the [analysis](https://github.com/devel0/iot-stepper-motor/blob/665799d9573e7852d47b750c6ae1fa2c27a3a87b/analysis/Program.cs#L307)

```csharp
var motion1_d = TimeSpan.FromMilliseconds(5000);
var motion1_rev_sec = 1d;

var motion2_d = TimeSpan.FromMilliseconds(2150);                
var motion2_rev_sec = 0d;
```

note: here only accel/decel considered (cruise not included)

![](data/img/analysis.png)

## Example01

### logic analyzer

![](data/img/example01_logic.png)

### video

[![](https://img.youtube.com/vi/dsn793ZnKGA/0.jpg)](https://youtu.be/dsn793ZnKGA)

### code

```cpp
// Example01
// - go to high speed 6rev/s in 0.25s
// - cruise speed for more 0.75s
// - go to low speed 0.1rev/s in 0.25s
// - cruise speed for more 0.25s

#include <mbed.h>

#include <number-utils.h>
#include <string-utils.h>
#include <timer-utils.h>
#include <slist.h>

#include <iot-scurve-stepper.h>

DigitalOut mPort(M1_PIN);

int main()
{
    printf("START\n");

    auto speed_change_time = 250ms;
    auto pulse_rev = 400;
    auto pulse_width = 5us;

    auto speed_up_time = 1000ms;
    auto speed_high_rps = 6.0;

    auto speed_down_time = 500ms;
    auto speed_low_rps = 0.1;

    //---

    Timer timer;

    SCurveStepper m(1, timer, mPort, pulse_rev, pulse_width);

    timer.start();

    auto t_start = timer.elapsed_time();

    auto motion_issued = false;
    auto stop_issued = false;

    while (true)
    {
        auto t_now = timer.elapsed_time();
        
        if (!motion_issued)
        {
            motion_issued = true;
            t_start = t_now;
            m.setSpeed(speed_high_rps, chrono_s(speed_change_time));
        }
        if (!stop_issued && t_now - t_start > speed_up_time)
        {
            stop_issued = true;
            m.setSpeed(speed_low_rps, chrono_s(speed_change_time));
        }
        if (t_now - t_start > speed_up_time + speed_down_time)
        {
            m.debugStats(true);
            
            motion_issued = stop_issued = false;
        }

        m.control();
    }
}
```

### plot (xlsx)

This [workbook](analysis/calc.xlsx) can be used to view accel,speed,pos graph from given input data ( s<sub>0</sub>, s<sub>d</sub>, d, x<sub>0</sub> ).

**accel**
![](data/img/example01_xlsx_accel.png)

**cruise**
![](data/img/example01_xlsx_cruise.png)

**decel**
![](data/img/example01_xlsx_decel.png)

### monitor

C-S-p `PlatformIO: Serial Monitor` ( or directly C-A-s )

```
m[1] pulse(exe/exp/max): 305/305/305   period_min: 209.000 ms   fMax: 4.785 kHz   pos: 1.980e5 step (∆:1.752e3)
m[1] pulse(exe/exp/max): 305/305/305   period_min: 209.000 ms   fMax: 4.785 kHz   pos: 1.997e5 step (∆:1.752e3)
m[1] pulse(exe/exp/max): 305/305/305   period_min: 209.000 ms   fMax: 4.785 kHz   pos: 2.015e5 step (∆:1.752e3)
m[1] pulse(exe/exp/max): 305/305/305   period_min: 209.000 ms   fMax: 4.785 kHz   pos: 2.032e5 step (∆:1.752e3)
m[1] pulse(exe/exp/max): 305/305/305   period_min: 209.000 ms   fMax: 4.785 kHz   pos: 2.050e5 step (∆:1.751e3)
m[1] pulse(exe/exp/max): 305/305/305   period_min: 209.000 ms   fMax: 4.785 kHz   pos: 2.067e5 step (∆:1.752e3)
```

notes:
- pulse(exe/exp/max) related to last part of motion ( decel )
- period_min, fMax measure minimum period and max pulse freq experienced from program start
- pos is the absolute position ( steps ) while ∆ is the diff from previous ( note that there are some difference caused by the cruise phase where there aren't control over the expected pulses but regulated by a timer; here there are some improvements todo )

## Examples wirings

from [examples/example.h](examples/example.h)

![nucleo64-F446RE.pdf](data/nucleo64-F446RE.pdf)

#### nucleo-144 **stm32-f767zi**

![nucleo144-F767ZI.pdf](data/nucleo144-F767ZI.pdf)

## TODO

- move by position
- control direction

## Debugging

- set example to start in [examples/example.h](examples/example.h) by setting a [define](https://github.com/devel0/iot-stepper-motor/blob/0a21b306125915c26b5c39f992fe5b0b4b8aa9dc/examples/example01/example.h#L6)
- start debugging using C-S-p `PlatformIO: Start Debugging`
- to debug examples/example01.cpp (included through [src/debug-main.cpp](src/debug-main.cpp)) it may needed to select only 1 platform from `platformio.ini` so the launch.json will generate accordingly; todo that comment others platform, ie:

```
[env]
src_filter = +<*> -<.git/> -<.svn/> -<example/> -<examples/> -<test/> -<tests/>

; [platformio]
; default_envs = nucleo_f446re, nucleo_f767zi

[env:nucleo_f767zi]
platform = ststm32
board = nucleo_f767zi
framework = mbed
test_build_project_src = true
debug_build_flags = -O0 -g -ggdb

; [env:nucleo_f446re]
; platform = ststm32
; board = nucleo_f446re
; framework = mbed
; test_build_project_src = true
; debug_build_flags = -O0 -g -ggdb
```

- [other references/troubleshoot](https://github.com/devel0/iot-stm32-ledblink-interrupt-debug#iot-stm32-ledblink-interrupt-debug)

## How this project was built

References:
- [Creating Library](https://docs.platformio.org/en/latest/librarymanager/creating.html?utm_medium=piohome&utm_source=platformio)
- [library.json](https://docs.platformio.org/en/latest/librarymanager/config.html)
