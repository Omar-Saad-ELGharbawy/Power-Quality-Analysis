function transient_signal = generate_transient_rho(transient_time,transient_amplitude,t1)

########################## Transient ####################################
rho = 14000;   % Impulsive transient decay factor
#t1            % Transient starting time
t2= t1 +0.001; % End Time

% Calculate the impulsive transient component
#transient_signal = transient_amplitude * exp(-rho * (transient_time - t1)) .* ( (transient_time >= t1));
transient_signal = transient_amplitude * exp(-rho * (transient_time - t1)) .* ( (transient_time >= t1) - (transient_time >= t2) );

endfunction
