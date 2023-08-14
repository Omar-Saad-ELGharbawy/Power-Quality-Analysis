function [freq,mag] = generatefou(y,fs)
  mag = (2*abs(fft(y))/length(y));
  freq = 0:fs/length(y):fs/2;
  mag = mag(1:length(y)/2+1);
  #plot(freq,mag);
endfunction
