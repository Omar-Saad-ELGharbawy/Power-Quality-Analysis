clc
clear all
close all

pkg load signal


% Define parameters
#Constant parameters
sampling_rate = 32000; % Sampling rate 32K Hz

######################## Time Signal #################
% Define time vector
duration = 0.2
t = linspace(0, duration, duration*sampling_rate);

########################  AC 50 HZ Power Supply Signal #########################################
ac_amplitude =220;
ac_frequency = 50; % Frequency of sinewave in Hz
theta_A = 0;  % Phase angle of sinewave

% Calculate the sinewave component
sinewave = ac_amplitude*sin(2*pi*ac_frequency*t + theta_A);

########################## Transient ####################################
t_m = 4000;         % Transient magnitude
rho = 14000;       % Impulsive transient decay factor
#t1 = 0.005;        % Transient starting time
t1 = 0.1;

% Calculate the impulsive transient component
impulsive_transient = t_m * exp(-rho * (t - t1)) .* (t >= t1);

########################################
% Calculate the overall signal
VA = sinewave + impulsive_transient;

########################################
% Plot the signal
figure;
subplot(3,2,1);
plot(t, impulsive_transient, 'b', 'LineWidth', 2);
xlabel('Time');
ylabel('Amplitude');
title('Impulsive Transient Only');
grid on;

subplot(3,2,2);
plot(t, sinewave, 'b', 'LineWidth', 2);
xlabel('Time');
ylabel('Amplitude');
title('Sine Wave');
grid on;

subplot(3,2,3);
plot(t, VA, 'b', 'LineWidth', 2);
xlabel('Time');
ylabel('VA');
title('Impulsive Transient Signal');
grid on;

[freq,mag] = generatefou(sinewave,sampling_rate);
subplot(3,2,4);
plot(freq,mag);
title('Sin')

[freq,mag] = generatefou(impulsive_transient,sampling_rate);
subplot(3,2,5);
plot(freq,mag);
title('Transient')

[freq,mag] = generatefou(VA,sampling_rate);
subplot(3,2,6);
plot(freq,mag);
title('Full')


% Save the figure
#saveas(gcf, '3_graphs.png');

