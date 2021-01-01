#ifndef UNIT_TEST

#include "example.h"

#ifdef EXAMPLE01

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

    auto wait_time = 2150ms;
    auto motion_time = 5000ms;
    auto speed_var_time = 1000ms;
    auto pulse_rev = 400;
    auto pulse_width = 5us;    
    auto speed_high_rps = 6.0;
    auto speed_low_rps = 1.5;

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
            m.setSpeed(speed_high_rps, chrono_s(speed_var_time));
        }
        if (!stop_issued && t_now - t_start > motion_time)
        {
            stop_issued = true;
            m.setSpeed(speed_low_rps, chrono_s(speed_var_time));
        }
        if (t_now - t_start > motion_time + wait_time)
        {
            m.debugStats();
            motion_issued = stop_issued = false;
        }         

        m.control();
    }
}

#endif

#endif
