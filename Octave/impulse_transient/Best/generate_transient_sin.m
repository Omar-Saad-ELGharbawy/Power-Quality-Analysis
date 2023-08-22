function transient_signal = generate_transient_sin(transient_time,transient_amplitude,transient_frequency,t1)

% Half sin signal with high frequency for transient signal
period = 1/transient_frequency; % period of one sine signal

half_sine = transient_amplitude * sin(2 * pi * transient_frequency * transient_time);
% Transient Window
half_period = 1 / (transient_frequency*2);

#t1=0.105; %t1 start duration
t2= t1 + half_period; %t2 end duration
ty= (t1+t2)/2; %ty decay time

rec_window = window(transient_time,t1,t2, 1);
exp_signal = exp(-transient_time/ty);

transient_signal= rec_window .*exp_signal .*half_sine;

endfunction
