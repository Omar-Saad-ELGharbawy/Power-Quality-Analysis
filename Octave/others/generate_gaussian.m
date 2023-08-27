function y = generate_gaussian(x, mu, width, peak)
  #x is the signal time
   sigma = (width) /pi;      % Standard deviation of the Gaussian distribution
   #sigma = 0.667;

    #y = exp(-0.5 * ((x - mu) / sigma).^2) / (sigma * sqrt(2 * pi));
    y = exp(-0.5 * ((x - mu) / sigma).^2);
endfunction
% Custom normpdf function

