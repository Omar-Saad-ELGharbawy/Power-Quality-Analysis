function mag = generateharmonics(mag, start, last, factor, frequency, fs, iter)
  phase_shift = 0;          % Phase shift in degrees
  for i = 2:iter
    [timetemp, magtemp] = generate_sin(start, last, factor, frequency * i, fs, phase_shift * i);
    mag = mag + (0.9 / i) * magtemp;
    % plot(timetemp, magtemp);  % Uncomment this line to plot individual harmonics
  endfor
endfunction


#function mag = generateharmonics(mag,start,last,factor,frequency,fs,iter)
#  %[time,mag] = generatesin(start,last,factor,frequency,fs);
#  for i = 2:iter
#    [timetemp,magtemp] = generate_sin(start,last,factor,frequency*i,fs);
#    mag = mag+(0.9/i)*magtemp;
#   %plot(time,mag);
#  endfor
#endfunction

