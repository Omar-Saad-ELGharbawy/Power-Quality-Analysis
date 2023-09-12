% generateWindow 
% Function Description :
% This function generates a rectangular window of amplitude A
% between t1 and t2
% Input Parameters :
% x is the signal time
% A is the signal Amplitude
% t1 is the start time of the window
% t2 is the end time of the window
% Output Parameters :
% rec_window is the rectangular window
function rec_window = generateWindow(x, t1,t2, A)
    rec_window= A .* ( (x>=t1) - (x>=t2) );
endfunction
