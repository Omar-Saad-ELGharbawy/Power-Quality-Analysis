function [time,mag] = generateharmonics(start,last,factor,frequency,fs,iter)
  [time,mag] = generatesin(start,last,factor,frequency,fs);
  for i = 2:iter
    [timetemp,magtemp] = generatesin(start,last,factor,frequency*i,fs);
    mag = mag+(1/i)*magtemp;
   %plot(time,mag);
  endfor
endfunction
