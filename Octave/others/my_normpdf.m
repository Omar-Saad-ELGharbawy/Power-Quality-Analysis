function y = my_normpdf(x, mu, sigma)
    #y = exp(-0.5 * ((x - mu) / sigma).^2) / (sigma * sqrt(2 * pi));
    y = exp(-0.5 * ((x - mu) / sigma).^2);
endfunction
% Custom normpdf function

