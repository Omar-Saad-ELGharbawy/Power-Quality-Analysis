function [time,mag]=generatesin(start,last,factor,frequency,fs)
  time = [start:1/fs:last];
  mag = factor*sin(2*pi*frequency*time);
  #plot(time,mag);
endfunction
