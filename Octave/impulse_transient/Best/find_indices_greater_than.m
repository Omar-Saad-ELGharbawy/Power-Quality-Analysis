function indices = find_indices_greater_than(value,array)
    indices = [];
    for i = 1:numel(array)
        if array(i) > value
            indices = [indices i];
        end
    end
end
