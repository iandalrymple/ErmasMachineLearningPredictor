import pandas as pd
import sklearn
import sys

def main():

    # Constants 
    arg_model_index = 2
    arg_trans_index = arg_model_index + 1
    arg_features_index = arg_trans_index + 1

    # Locals 
    logs = []
    error_occurred = 0
    predictions = 0.0

    try:

        print("we are here")
        return

        # Delete later - keeping around for debugging.
        # raw_frame = pd.read_pickle(R"C:\Users\ianda\source\GitHub\ErmasMachineLearningPredictor\Ignore\MODELS\PROCESSED_FRAME_UTICA.pkl")
        # model = pd.read_pickle(R"C:\Users\ianda\source\GitHub\ErmasMachineLearningPredictor\Ignore\MODELS\GBR_UTICA.pkl")
        # col_transformer = pd.read_pickle(R"C:\Users\ianda\source\GitHub\ErmasMachineLearningPredictor\Ignore\TRANSFORMER\COL_TRANSFORMER.pkl")

        # Kick off log.
        logs.append("Starting application.")

        # Parse the args 
        model_path = sys.argv[arg_model_index]
        trans_path = sys.argv[arg_trans_index]
        features_path = sys.argv[arg_features_index]

        # Add args to logs.
        logs.append(model_path)
        logs.append(trans_path)
        logs.append(features_path)

        # Read in the pickles.
        model =  pd.read_pickle(model_path)
        col_transformer = pd.read_pickle(trans_path)

        # Read in the features.
        features = pd.read_csv(features_path)

        # Transform the features.
        transformed = col_transformer.fit_transform(features)

        # Finally peform the prediction.
        predictions = pd.DataFrame(model.predict(transformed))

    except Exception as ex:
        
        template = "An exception of type {0} occurred with arguments:\n{1!r}"
        message = template.format(type(ex).__name__, ex.args)
        logs.append(message)
        error_occurred = 1
   
    # finally:
         
    #     try:
    #         print("!!!!!" + ConstructReturnJson(error_occurred, predictions[0][0], logs) + "!!!!!")
    #     except:
    #         pass

def ConstructReturnJson(error_occurred, prediction, logs: list) -> str:
    
    # Opening and closing {} need to be added after as they mess up the .format method
    json = "\"error\" : {0},"
    json += "\"prediction\" : \"{1}\""  
    json = json.format(error_occurred, prediction)
    if(len(logs) > 0):
        json += ",\"logs\":["
        for i in range(len(logs)):
            json += "\"" + logs[i] + "\","
        json = json[:-1]
        json += "]"
    final_json = "{" + json + "}"
    return final_json

if __name__ == "__main__":
    main()