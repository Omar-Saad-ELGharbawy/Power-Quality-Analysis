function [freq,full_mag,mag] = generatefou(y,fs)
  full_mag = fft(y);
  mag = (2*abs(full_mag)/length(y));
  freq = 0:fs/length(y):fs/2;
  mag = mag(1:length(y)/2+1);
  %plot(freq,mag);
endfunction
