function [ transients_values , transients_times ]= ExtractTransients(values_buffer, time_buffer, startIndices, threshold)
    transients_values = {};
    transients_times = {};
    for i = 1:length(startIndices)
        value = [];
        time = [];
        j = startIndices(i);
        while (values_buffer(j) <= threshold)
            j = j + 1;
        end

        % if(values_buffer(j) > threshold)
        %     value = [value values_buffer(j)];
        %     time = [time time_buffer(j)];
        % end
        % j = j + 1;
        while (values_buffer(j) > threshold) 
            value = [value values_buffer(j)];
            time = [time time_buffer(j)];
            j = j + 1;
        end
        if ~isempty(value)
            transients_values = [transients_values; value];
        if ~isempty(time)
            transients_times = [transients_times; time];
        end
    end
end
