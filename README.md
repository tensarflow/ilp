# ILP

The aim of this work is to determine how errors in additive manufacturing can be detected in time. To this end, the following research question is asked: Can errors of the Fused Deposition Layer (FDM) printing process be detected by the analysis of the electrical power? In order to answer the research question, an investigation of the state-of-the-art for pattern detection was carried out and then a strategy was developed to implement the appropriate methods. It has been found that the detection of anomalies in the measurement of power of a 3D printing process is possible, but deeper research in the field of identification of events of stepper motors must be performed, since the performance behaviour is almost the same at both standstill and during movement of the motors. A successful system was implemented, which allows to train new patterns, to detect them in a data stream and to detect certain anomalies in that stream.

# Installtion Instruction ILP

1. Clone the repository
2. Open the `.sln` file with Visual Studio 2017
3. Set `ILP` as Startup project
4. Build and run the solution

# Usage

1. Click `Start System` to start the CEP engine.
2. Click `Train Data` to train a new Pattern.