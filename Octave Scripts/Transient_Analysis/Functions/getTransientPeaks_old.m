% getTransientPeaks_old 
% Function Description :
%	This function is used to find the transient peaks in a given signal. The algorithm works as follows:
function peak_values = getTransientPeaks_old(transients_values);
peak_values = [];
for i = 1:numel(transients_values)
    values = transients_values{i};
    if ~isempty(values)
        row_peak = max(values);
    else
        row_peak = 0;
    end
    peak_values = [peak_values; row_peak];
end
endfunction
