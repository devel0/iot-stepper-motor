#ifndef UNIT_TEST

#include "example.h"

#ifdef EXAMPLE01

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

#endif

#endif
