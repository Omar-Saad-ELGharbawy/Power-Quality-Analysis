function rec_window = window(x, t1,t2, A)
  % x is the signal time
  % t is the split time
  % A is the signal Amplitude
  % t1 is the start time of the window
  % t2 is the end time of the window
    rec_window= A .* ( (x>=t1) - (x>=t2) );
endfunction
