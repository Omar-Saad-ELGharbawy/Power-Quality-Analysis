% getTransientWidths 
% Function Description :
%   This function calculates the width of each transient in the transients_values cell array
%   and returns a vector of widths.
% Inputs :
%   transients_values : A cell array of transients values
% Outputs:
%   widths : A vector of transient widths   
function widths = getTransientWidths(transients_values)
widths = [];
for i = 1:numel(transients_values)
    values = transients_values{i};
    row_width = length(values) * 3.125e-05;
    widths = [widths; row_width];
end
endfunction

