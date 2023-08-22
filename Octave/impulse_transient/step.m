clc
clear all
close all

sampling_rate=33000;
duration = 5;

t=0:1/sampling_rate:5;

t = linspace(0, duration, duration * sampling_rate);

t1 = 1;
t2 = 2;

unit_step_1 = (t>=1);
#unit_step_1 = (t-t1);

#stem(n,x);
subplot(3,1,1);
plot(t, unit_step_1);

unit_step_2 = (t>=2);
subplot(3,1,2);
plot(t, unit_step_2);

#rec_window = unit_step_1 - unit_step_2;
rec_window = window(t,t1,t2, 1);

subplot(3,1,3);
plot(t, rec_window);
