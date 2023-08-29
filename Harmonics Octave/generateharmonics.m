function mag = generateharmonics(mag,start,last,factor,frequency,fs,iter)
  %[time,mag] = generatesin(start,last,factor,frequency,fs);
  for i = 2:iter
    [timetemp,magtemp] = generatesin(start,last,factor,frequency*i,fs);
    mag = mag+magtemp;
   %plot(time,mag);
  endfor
endfunction
