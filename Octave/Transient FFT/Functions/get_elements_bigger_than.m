function newArray = get_elements_bigger_than(value,originalArray)

newArray = [];
for i = 1:length(originalArray)
    if originalArray(i) > 220
        newArray = [newArray, originalArray(i)];
    end
end

endfunction
