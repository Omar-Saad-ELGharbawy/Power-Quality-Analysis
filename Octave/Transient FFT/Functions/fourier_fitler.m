function [filtered_signal] = fourier_fitler(signal,Fs,cut_off)
  # Get Fourier Domain
  [frequencies,magnitude,phase] = generate_all_fourier(signal,Fs);
  # Low pass fourier filter
  % Find the indices corresponding to the desired frequencies
  cutoff_index_ = find(frequencies >= cut_off);
  minus_cutoff_index = find(frequencies <= -cut_off);
  % Clear Power at this frequencies
  magnitude(cutoff_index_) = 0;
  magnitude(minus_cutoff_index) = 0;
  # Inverse Fourier Transform
  [filtered_signal] = get_inverse_fourier(magnitude , phase);
endfunction
