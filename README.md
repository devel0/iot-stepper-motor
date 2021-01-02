# iot-scurve-stepper-motor

## Description

:warning: this is a work in progress

## Quickstart

Install using vscode command palette `PlatformIO: New Terminal` referring to [platformio registry]() cli mode doc.

If got trouble during compile, remove `.pio/libdeps/nucleo_f446re/iot-scurve-stepper-motor/library.json`.

## Examples

```cpp
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

    auto speed_up_time = 2000ms;
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

        auto m_state = m.state();

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
            m.debugStats();
            motion_issued = stop_issued = false;
        }

        m.control();
    }
}
```

![](data/img/example01.png)

## Analysis

```sh
git submodule update --init --recursive
cd analysis
dotnet run
```

![](data/img/analysis.png)

## Debugging

to debug examples/example01.cpp (included through [src/debug-main.cpp](src/debug-main.cpp)) it may needed to select only 1 platform from `platformio.ini` so the launch.json will generate accordingly; todo that comment others platform, ie:

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
