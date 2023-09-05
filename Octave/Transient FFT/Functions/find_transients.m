function [transients_count , transients_start_indices ] = find_transients(signalBuffer, threshold)
    transients_count = 0;
    transients_start_indices = [];
    isTransient = false;

    for i = 1:length(signalBuffer)
        if signalBuffer(i) > threshold
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

