function [time, signal] = generate_sine_wave(freq, sample_rate, my_duration, amplitude, phase)
    time = linspace(0, my_duration, my_duration * sample_rate);
    signal = amplitude * sin(2 * pi * freq * time + phase);
endfunction
