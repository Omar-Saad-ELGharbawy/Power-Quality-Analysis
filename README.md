# PowerQuality Analysis 
***

## Table of contents
* [Introduction](#introduction)
* [Project Structure](#project-structure)
* [GUI](#gui)
* [Technologies](#technologies)

## Introduction

This project applies different Digital Signal processing Algorithms for Power Quality AC Signals to Simulate and Analyse:
- Harmonics.
- Transient signals.

The Algorithms are written in Octave and then converted to C++ to be used in the GUI. The Algorithms are also optimized to run on GPU using CUDA.

### What is Harmonics

### What is Transient
Transients in power quality signals are sudden and temporary changes characterized by voltage or current variations. They can result from switching operations, lightning strikes, or equipment faults. These transients can lead to harmful effects such as equipment damage, data corruption, and increased downtime. Detecting and analyzing transients helps in safeguarding equipment, improving reliability, minimizing downtime, and ensuring compliance with power quality standards.

- Signal with transients
![Signal with Multiple Transients](https://github.com/Omar-Saad-ELGharbawy/PowerQuality_GUI/assets/84602951/5ca919a0-d402-45cc-8962-0e5bf8d3dffe)

- Transients Localization using STFT
![STFT](https://github.com/Omar-Saad-ELGharbawy/PowerQuality_GUI/assets/84602951/b5ce517f-f2c9-48a4-b0d5-5db22e5f546f)

## Project Structure

The project is divided into 3 main parts :
- WPF GUI : A Graphical User Interface to display the results of the Algorithms.
- Octave Scripts : The Algorithms are written in Octave and then converted to C++.
- GPU Optimizations : The Algorithms are optimized to run on GPU using CUDA.
  
```
PowerQuality Analysis
├─  WPF GUI
│  ├─  Mainwindow (Harmonics WPF Window)
│  ├─  Secondwindow (Transient WPF Window)
│  ├─  Transient Algorithms
│     ├─  SignalGenerator.cs
│     ├─  TransientDetection.cs
│     └─  FourierFilters.cs
│  └─  Harmonics Algorithms
├─  Octave Scripts
│  ├─  Harmonics
│  └─ Transient Analysis
├─  GPU Optimizations
│  ├─ GPU_info
│  └─ Convolution
README.md
```

## GUI 
The GUI is written in C# WPF and it is divided into 2 main parts :

- Harmonics Window

![Harmonics Window](https://github.com/Omar-Saad-ELGharbawy/PowerQuality_GUI/assets/84602951/a51e673e-e9aa-4290-83bd-bbe82a79cbc8)

- Transient Window

![Transient Window](https://github.com/Omar-Saad-ELGharbawy/PowerQuality_GUI/assets/84602951/fc045f09-0362-4d2e-affd-68ee373c455b)


## Technologies

### Development Technologies
Project is created with:
* C# .NET Framework
* C# WPF
* Octave
* CUDA 
* Visual Studio 
* Visual Studio Code

### Scientific Technologies
* OOP
* Signal Processing
* Digital Signal Processing
* Fourier Analysis
* Short Time Fourier Transform Analysis
* GPU Programming

### Team
Biomedical Engineers :
- [Omar_Saad](https://github.com/Omar-Saad-ELGharbawy)
- [Ahmed_Hassan](https://github.com/ahmedhassan187)

