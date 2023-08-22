clc
clear all
close all

x= 0:0.001:1;
y=sin(2*pi*x);

subplot(2,1,1)
plot(x,y)
#xlabel('time')
#ylabel('Amplitude')

subplot(2,1,2)
stem(x,y)
