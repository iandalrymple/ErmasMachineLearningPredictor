import pandas as pd
import sklearn
import sys

def main():

    # Constants 
    arg_model_index = 1
    arg_trans_index = arg_model_index + 1
    arg_features_index = arg_trans_index + 1

    # Locals 
    logs = []
    error_occurred = 0

    try:

        # Delete later - keeping around for debugging.
        # raw_frame = pd.read_pickle(R"C:\Users\ianda\source\GitHub\ErmasMachineLearningPredictor\Ignore\PROCESSED_FRAME_UTICA.pkl")
        model = pd.read_pickle(R"C:\Users\ianda\source\GitHub\ErmasMachineLearningPredictor\Ignore\GBR_UTICA.pkl")
        # col_transformer = pd.read_pickle(R"C:\Users\ianda\source\GitHub\ErmasMachineLearningPredictor\Ignore\COL_TRANSFORMER_UTICA.pkl")

        # Parse the args 
        model_path = sys.argv[arg_model_index]
        trans_path = sys.argv[arg_trans_index]
        features_path = sys.argv[arg_features_index]

        # Read in the pickles.
        model =  pd.read_pickle(model_path)
        col_transformer = pd.read_pickle(trans_path)

        # Read in the features.
        features = pd.read_csv(features_path)

        # Transform the features.
        transformed = col_transformer.fit_transform(features)

        # Finally peform the prediction.
        predictions = pd.DataFrame(model.predict(transformed))
        print(predictions.head())

    except Exception as ex:
        
        template = "An exception of type {0} occurred. Arguments:\n{1!r}"
        message = template.format(type(ex).__name__, ex.args)
        logs.append(message)
        error_occurred = 1
   
    finally:
         
        print("fixing to exit")

if __name__ == "__main__":
    main()