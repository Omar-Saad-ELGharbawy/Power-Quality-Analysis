function unit_signal = unit_step(x, t, A)
  % x is the signal time
  % t is the split time
  % A is the signal Amplitude
    unit_signal= A* (x>=t);
endfunction
