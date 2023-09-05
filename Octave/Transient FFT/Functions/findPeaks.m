function [positivePeak, negativePeak] = findPeaks(signal)
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
end
