function [time,mag] = generateinterharmonics(start,last,factor,frequency,fs,iter)
  [time,mag] = generatesin(start,last,factor,frequency,fs);
  for i = 2:iter
    %f = frequency/i;
    [timetemp,magtemp] = generatesin(start,last,factor,(frequency/50)*i,fs);
    mag = mag+(1/10)*magtemp;
   endfor
endfunction