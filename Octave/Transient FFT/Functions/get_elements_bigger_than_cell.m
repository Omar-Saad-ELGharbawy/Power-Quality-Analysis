function [filtered_values,filtered_times] = get_elements_bigger_than_cell(threshold, values_array,times_array)
    filtered_values = cell(size(values_array));
    filtered_times = cell(size(times_array));
    for i = 1:numel(values_array)
        values = values_array{i};
        times = times_array{i};
        filtered_values{i} = values(values > threshold);
        filtered_times{i} = times(values > threshold);
    end
end
