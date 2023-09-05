function transientCount = CalculateTransientCount(signalBuffer, threshold)
    transientCount = 0;
    isTransient = false;

    for i = 1:length(signalBuffer)
        if signalBuffer(i) > threshold
            if ~isTransient
                % Start of a new transient
                isTransient = true;
                transientCount = transientCount + 1;
            end
        else
            isTransient = false;
        end
    end
end
