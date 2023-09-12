% generate_transient_signal  
% Function Description :
%   This function generates a transient signal with a given transient time,
%   transient amplitude, transient frequency and t1 start duration.
%   The transient signal is a half sine signal with a high frequency.
%   The transient signal is multiplied with a rectangular window and an
%   exponential decay function.
%   INPUTS:
%       transient_time: time of the transient signal
%       transient_amplitude: amplitude of the transient signal
%       transient_frequency: frequency of the transient signal
%       t1: start duration of the transient signal
%   OUTPUTS:
%       transient_signal: transient signal
function transient_signal = generateTransientSignal(transient_time,transient_amplitude,transient_frequency,t1)

% Half sin signal with high frequency for transient signal
period = 1/transient_frequency; % period of one sine signal
half_sine = transient_amplitude * sin(2 * pi * transient_frequency * transient_time);
% Transient Window
half_period = 1 / (transient_frequency*2);

#t1=0.105; %t1 start duration
t2= t1 + half_period; %t2 end duration
ty= (t1+t2)/2; %ty decay time

rec_window = generateWindow(transient_time,t1,t2, 1);
exp_signal = exp(-transient_time/ty);

transient_signal= rec_window .*exp_signal .*half_sine;

endfunction
