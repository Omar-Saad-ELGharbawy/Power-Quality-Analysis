
% Parameters
mu = 0;         % Mean of the Gaussian distribution
sigma = 1;      % Standard deviation of the Gaussian distribution
duration = 5;   % Duration of the signal in seconds
sampling_rate = 32000;  % Sampling rate (samples per second)

% Generate time axis
t = -duration : 1/sampling_rate : duration;

subplot(2,1,1)
% Generate Gaussian signal using custom normpdf function
gaussian_signal = my_normpdf(t, mu, sigma);

% Plot the signal
plot(t, gaussian_signal);
#xlabel('Time (s)');
#ylabel('Amplitude');
#title('Gaussian Signal');

#subplot(3,1,2)

% Set the parameters
frequency = 50; % Frequency in Hz
amplitude = 220; % Amplitude of the signal
duration = 5; % Duration of the signal in seconds
sampling_rate = 32000; % Sampling rate 32K Hz
t = linspace(-duration, duration, duration * sampling_rate);

% Generate the sine signal
signal = amplitude * sin(2 * pi * frequency * t);

subplot(3,1,2)
% Plot the signal
plot(t, signal);
#xlabel('Time (s)');
#ylabel('Amplitude');
#title('Sine Signal with 32K Hz Frequency');

#Transient Signal
transient_signal = signal +  gaussian_signal

subplot(3,1,3)
% Plot the signal
plot(t, signal);
#subplot(2,1,2)

#plot(t, transient_signal);



