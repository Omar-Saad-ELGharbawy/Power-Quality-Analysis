clc
clear all
close all

% Define parameters
#Constant parameters
sampling_rate = 32000; % Sampling rate 32K Hz

######################## Time Signal #################
% Define time vector
duration = 0.02
t = linspace(0, duration, duration*sampling_rate);

########################  AC 50 HZ Power Supply Signal #########################################
ac_amplitude =1;
ac_frequency = 50; % Frequency of sinewave in Hz
theta_A = 0;  % Phase angle of sinewave

% Calculate the sinewave component
sinewave = ac_amplitude*sin(2*pi*ac_frequency*t + theta_A);

########################## Transient ####################################
t_m = 2;         % Transient magnitude
rho = 14000;       % Impulsive transient decay factor
t1 = 0.005;        % Transient starting time

% Calculate the impulsive transient component
impulsive_transient = t_m * exp(-rho * (t - t1)) .* (t >= t1);

########################################
% Calculate the overall signal
VA = sinewave + impulsive_transient;

########################################
% Plot the signal
figure;
subplot(3,1,1);
plot(t, impulsive_transient, 'b', 'LineWidth', 2);
xlabel('Time');
ylabel('Amplitude');
title('Impulsive Transient Only');
grid on;

subplot(3,1,2);
plot(t, sinewave, 'b', 'LineWidth', 2);
xlabel('Time');
ylabel('Amplitude');
title('Sine Wave');
grid on;

subplot(3,1,3);
plot(t, VA, 'b', 'LineWidth', 2);
xlabel('Time');
ylabel('VA');
title('Impulsive Transient Signal');
grid on;

% Save the figure
saveas(gcf, '3_graphs.png');

