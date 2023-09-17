function [time, signal] = generateSignals(sampling_rate, start_time, end_time, amplitude, frequency, phase_shift, harmonics_num, transient_params)

    % Generate AC signal
    [ac_time, ac_signal] = generateSin(start_time, end_time, amplitude, frequency, sampling_rate, phase_shift);

    % Generate Harmonics signal
    harmonics = generateHarmonics(ac_signal, start_time, end_time, amplitude, frequency, sampling_rate, harmonics_num);

    % Generate Transients Signals
    transient_signal = zeros(size(ac_signal));
    for i = 1:length(transient_params)
        transient_time = transient_params(i).time;
        transient_freq = transient_params(i).frequency;
        transient_amp = transient_params(i).amplitude;
        transient = generateTransientSignal(ac_time, transient_amp, transient_freq, transient_time);
        transient_signal += transient;
    end

    % Combine all signals
    all_signals = ac_signal + harmonics + transient_signal;

    % Return time and signal arrays
    time = ac_time;
    signal = all_signals;

end
