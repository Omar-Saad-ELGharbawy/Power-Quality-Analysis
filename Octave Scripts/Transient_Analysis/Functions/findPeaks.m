% findPeaks Function Description : 
% This function finds the transient peak of a signal by adding positive peak and negative peak from filtered subtracted signal
% Inputs :
%       signal : The signal from which the transient peak is to be found    
% Outputs :
%       transient_peak : The transient peak of the signal
function transient_peak = findPeaks(signal)
    % Initialize variables
    positivePeak = -Inf;
    negativePeak = Inf;
    % Iterate through each element in the signal
    for i = 1:length(signal)
        % Check if the current element is a positive peak
        if signal(i) > positivePeak
            positivePeak = signal(i);
        end
        % Check if the current element is a negative peak
        if signal(i) < negativePeak
            negativePeak = signal(i);
        end
    end
    transient_peak = positivePeak - negativePeak;
end
