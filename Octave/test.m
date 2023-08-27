clc
clear all
close all

amplitude = 220;
frequency = 50; % Hz

time = 0.005; % seconds

start_point = 0;


magnitude = amplitude * sin(2 * pi * frequency * time + start_point);
disp(magnitude);
