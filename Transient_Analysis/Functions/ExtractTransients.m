% ExtractTransients
% Function Description : 
%   Extracts the transients sub signals from the values_buffer and time_buffer
%   according to the startIndices and threshold.
% Inputs :
%   values_buffer : signal in time domain containig transients
%   time_buffer : time vector corresponding to the values_buffer
%   startIndices : vector containing the indices of the start of each transient
%   threshold : threshold value to determine the transient values
% Outputs:
%   transients_values : cell array containing the transients values
%   transients_times : cell array containing the transients times
function [ transients_values , transients_times ]= ExtractTransients(values_buffer, time_buffer, startIndices, threshold)
    transients_values = {};
    transients_times = {};
    for i = 1:length(startIndices)
        value = [];
        time = [];
        j = startIndices(i);        
        % Find the first value above the threshold
        while (values_buffer(j) <= threshold)
            j = j + 1;
        end
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
