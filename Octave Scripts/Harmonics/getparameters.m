function [freqs,mags] = getparameters(mag,fundmental_freq,fs)
  [freq,full_mag,mag] = generatefou(mag,fs);
  freqs = {};
  mags ={};
  j=1;
  for i=1:fs
    if rem(i,fundmental_freq) == 0
      ind = find((i-0.09)<=freq&freq<=(i+0.09));
      %disp(ind);
      if ~(isempty(ind))
          freqs{j} = freq(ind);
          mags{j} = abs(mag(ind)); % Assuming mag is a cell array
          j = j + 1;
      end
    end
  endfor
endfunction
