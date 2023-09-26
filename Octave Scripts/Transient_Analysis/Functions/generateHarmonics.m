% generateHarmonics Function Description : 
% Generates harmonics of a given frequency and adds them to the signal
% Inputs :
% signal : The signal to which harmonics are to be added
% start : The start time of the signal
% last : The end time of the signal
% factor : The factor by which the signal is to be scaled
% frequency : The frequency of the signal
% fs : The sampling frequency of the signal
% iter : The number of harmonics to be added
% Outputs :
% signal : The signal with harmonics added
function signal = generateHarmonics(signal, start, last, factor, frequency, fs, iter)
  phase_shift = 0;          % Phase shift in degrees
  for i = 2:iter
    [time_temp, mag_temp] = generateSin(start, last, factor, frequency * i, fs, phase_shift * i);
    signal = signal + (0.9 / i) * mag_temp;
    % plot(timetemp, magtemp);  % Uncomment this line to plot individual harmonics
  endfor
endfunction
