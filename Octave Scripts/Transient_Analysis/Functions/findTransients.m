% findTransients
% Function Description :  Finds the transients in the given STFT magnitude
% Input Parameters :
%   stft_magnitude : The mean magnitudes of the STFT frequencies
%   threshold : The threshold above which a transient is considered to be present
% Returns :
%   transients_count : The number of transients in the given STFT
%   transients_start_indices : The indices of the start of the transients
function [transients_count , transients_start_indices ] = findTransients(stft_magnitude, threshold)
    transients_count = 0;
    transients_start_indices = [];
    isTransient = false;
    % Loop over stft magnitude to find start of indexes more than the threshold
    for i = 1:length(stft_magnitude)
        if stft_magnitude(i) > threshold
            if ~isTransient
                % Start of a new transient
                isTransient = true;
                % Add the start index to the list
                transients_start_indices = [transients_start_indices i];
                transients_count = transients_count + 1;
            end
        else
            isTransient = false;
        end
    end
end

