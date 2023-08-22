% Define the time vector
t = linspace(0, 1, 1000);

% Generate a step function transient signal
signal = zeros(size(t));
signal(t >= 0.2 & t < 0.8) = 1;

% Plot the transient signal
plot(t, signal);
xlabel('Time');
ylabel('Amplitude');
title('Step Function Transient Signal');
